using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Stump.Core.Reflection
{
    /// <summary>
    /// Generates field accessor methods
    /// </summary>
    public class AccessorBuilder
    {
        #region Delegates

        /// <summary>
        /// Returns field value.
        /// Delegate is bound to specific instance.
        /// </summary>
        /// <returns>Field value</returns>
        public delegate object GetFieldValueBoundDelegate();

        /// <summary>
        /// Returns field value.
        /// Delegate is not bound to any specific instance.
        /// </summary>
        /// <returns>Field value</returns>
        public delegate object GetFieldValueUnboundDelegate(object instance);

        /// <summary>
        /// Modifies field value
        /// Delegate is bound to specific instance.
        /// </summary>
        public delegate void SetFieldValueBoundDelegate(object value);

        /// <summary>
        /// Modifies field value
        /// Delegate is not bound to any specific instance.
        /// </summary>
        public delegate void SetFieldValueUnboundDelegate(object instance, object value);

        #endregion

        //note: first argument for bound delegates is the object itself,
        //and we generate methods that receives this first parameter even if delegate signature "omits" it

        private static DynamicMethod CreateGetterImpl(Type instanceType, Type fieldType, string fieldName)
        {
            if (fieldName == null)
                throw new ArgumentNullException("fieldName");

            if (fieldName.Length == 0)
                throw new ArgumentException("Field name must be non-empty string");

            FieldInfo fInfo = instanceType.GetField(fieldName,
                                                  BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            if (fInfo == null)
                throw new MissingFieldException("Can't obtain field " + fieldName + " from class " + instanceType.Name);

            if (!fieldType.IsAssignableFrom(fInfo.FieldType))
                throw new InvalidCastException("Field " + fieldName + " of type " + fInfo.FieldType +
                                               " can not be casted to " + fieldType.FullName);

            //buid getter
            var getterDef = new DynamicMethod(instanceType.Name + "_" + fieldName + "_Getter",
                                              fieldType,
                                              new[] { instanceType },
                                              instanceType);


            ILGenerator getterIL = getterDef.GetILGenerator();

            getterIL.Emit(OpCodes.Ldarg_0); //we expect in top of the stack object instance
            getterIL.Emit(OpCodes.Ldfld, fInfo);
            getterIL.Emit(OpCodes.Ret);

            return getterDef;
        }

        private static DynamicMethod CreateSetterImpl(Type instanceType, Type fieldType, string fieldName)
        {
            if (fieldName == null)
                throw new ArgumentNullException("fieldName");

            if (fieldName.Length == 0)
                throw new ArgumentException("Field name must be non-empty string");

            FieldInfo fInfo = instanceType.GetField(fieldName,
                                                  BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            if (fInfo == null)
                throw new MissingFieldException("Can't obtain field " + fieldName + " from class " + instanceType.Name);

            if (!fieldType.IsAssignableFrom(fInfo.FieldType))
                throw new InvalidCastException("Field " + fieldName + " of type " + fInfo.FieldType +
                                               " can not be casted to " + fieldType.FullName);

            //build setter
//            var setterDef = new DynamicMethod(instanceType.Name + "_" + fieldName + "_Setter",
//                                              typeof (void),
//                                              new[] { typeof(object), typeof(object) },
//                                              instanceType);

//            ILGenerator setterIL = setterDef.GetILGenerator();
//            setterIL.Emit(OpCodes.Ldarg_0); //we expect in top of the stack object instance
//            setterIL.Emit(OpCodes.Ldarg_1);
//            setterIL.Emit(OpCodes.Stfld, fInfo);
//            setterIL.Emit(OpCodes.Ret); //note: we have to return, even if method is void

            var setterDef = new DynamicMethod(
                instanceType.Name + "_" + fieldName + "_Setter"
                , typeof(void),
                new[] { typeof(object), typeof(object) },
                instanceType);

            var generator = setterDef.GetILGenerator();
            var local = generator.DeclareLocal(fInfo.DeclaringType);
            generator.Emit(OpCodes.Ldarg_0);
            if (fInfo.DeclaringType.IsValueType)
            {
                generator.Emit(OpCodes.Unbox_Any, fInfo.DeclaringType);
                generator.Emit(OpCodes.Stloc_0, local);
                generator.Emit(OpCodes.Ldloca_S, local);
            }
            else
            {
                generator.Emit(OpCodes.Castclass, fInfo.DeclaringType);
                generator.Emit(OpCodes.Stloc_0, local);
                generator.Emit(OpCodes.Ldloc_0, local);
            }
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(fInfo.FieldType.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, fInfo.FieldType);
            generator.Emit(OpCodes.Stfld, fInfo);
            generator.Emit(OpCodes.Ret);

            return setterDef;
        }

        /// <summary>
        /// Creates unbound delegate that allows object field read operation.
        /// </summary>
        /// <param name="fieldName">Name of the field</param>
        /// <returns>Delegate allowing field value reading</returns>
        public static GetFieldValueUnboundDelegate CreateGetter(Type instanceType, Type fieldType, string fieldName)
        {
            GetFieldValueUnboundDelegate getter;

            DynamicMethod getterDef = CreateGetterImpl(instanceType, fieldType, fieldName);

            getter =
                (GetFieldValueUnboundDelegate)
                DynamicExtension.CreateDelegate(getterDef, typeof (GetFieldValueUnboundDelegate));

            return getter;
        }

        /// <summary>
        /// Creates bound delegate that allows object field read operation.
        /// </summary>
        /// <param name="instance">Instance of the object that delegate will be bound to</param>
        /// <param name="fieldName">Name of the field</param>
        /// <returns>Delegate allowing field value reading</returns>
        public static GetFieldValueBoundDelegate CreateGetter(object instance, Type fieldType, string fieldName)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            DynamicMethod getterDef = CreateGetterImpl(instance.GetType(), fieldType, fieldName);

            var getter =
                (GetFieldValueBoundDelegate)
                getterDef.CreateDelegate(typeof (GetFieldValueBoundDelegate), instance);

            return getter;
        }

        /// <summary>
        /// Creates unbound delegate that allows object field write operation.
        /// </summary>
        /// <param name="fieldName">Name of the field</param>
        /// <returns>Delegate allowing field value writing</returns>
        public static SetFieldValueUnboundDelegate CreateSetter(Type instanceType, Type fieldType, string fieldName)
        {
            SetFieldValueUnboundDelegate setter = null;
            DynamicMethod setterDef = CreateSetterImpl(instanceType, fieldType, fieldName);

            setter =
                (SetFieldValueUnboundDelegate)
                DynamicExtension.CreateDelegate(setterDef, typeof (SetFieldValueUnboundDelegate));

            return setter;
        }

        /// <summary>
        /// Creates bound delegate that allows object field write operation.
        /// </summary>
        /// <param name="instance">Instance of the object that delegate will be bound to</param>
        /// <param name="fieldName">Name of the field</param>
        /// <returns>Delegate allowing field value writing</returns>
        public static SetFieldValueBoundDelegate CreateSetter(object instance, Type fieldType, string fieldName)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            SetFieldValueBoundDelegate setter = null;
            DynamicMethod setterDef = CreateSetterImpl(instance.GetType(), fieldType, fieldName);

            setter =
                (SetFieldValueBoundDelegate)
                setterDef.CreateDelegate(typeof (SetFieldValueBoundDelegate), instance);

            return setter;
        }
    }
}
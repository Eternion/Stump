namespace Stump.DofusProtocol.Enums
{
	public 	enum IdentificationFailureReasonEnum
	{
		BAD_VERSION=1,
		WRONG_CREDENTIALS=2,
		BANNED=3,
		KICKED=4,
		IN_MAINTENANCE=5,
		TOO_MANY_ON_IP=6,
		TIME_OUT=7,
		UNKNOWN_AUTH_ERROR=99,
	}
}

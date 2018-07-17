namespace DFC.Digital.Repository.ONET.Helper
{
    using System.Runtime.Serialization;

    public class SqlDbException : DbException
    {
        public SqlDbException ( ErrorReason errorReason , int extendedReason = 0 ) : base ( errorReason , extendedReason )
        {
        }

        public SqlDbException ( ErrorReason errorReason , string message , int extenedReason = 0 ) : base ( errorReason , message , extenedReason )
        {
        }

        public SqlDbException ( ErrorReason errorReason , SerializationInfo info , StreamingContext context , int extenedReason = 0 ) : base ( errorReason , info , context , extenedReason )
        {
        }

        public SqlDbException ( ErrorReason errorReason , string message , System.Exception exception , int extenedReason = 0 ) : base ( errorReason , message , exception , extenedReason )
        {
        }
    }
}

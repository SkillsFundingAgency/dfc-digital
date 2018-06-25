namespace DFC.Digital.Repository.ONET.Helper
{
    using System;
    using System.Runtime.Serialization;

    public abstract class DbException : System.Data.Common.DbException
    {
        public enum ErrorReason
        {
            Configuration,
            Connection,
            ConstraintViolation,
            OptimisticConcurrency,
            InsertUpdateFailure,
            Timeout,
            SecurityViolation,
            ExtendedReason
        }

        protected DbException ( ErrorReason errorReason , int extendedReason = 0 )
        {
            Reason = errorReason;
            ExtendedReason = extendedReason;
        }

        protected DbException ( ErrorReason errorReason , String message , int extenedReason = 0 )
            : base ( message )
        {
            Reason = errorReason;
            ExtendedReason = extenedReason;

        }

        protected DbException ( ErrorReason errorReason , SerializationInfo info , StreamingContext context , int extenedReason = 0 )
            : base ( info , context )
        {
            Reason = errorReason;
            ExtendedReason = extenedReason;

        }

        protected DbException ( ErrorReason errorReason , String message , System.Exception exception , int extenedReason = 0 )
            : base ( message , exception )
        {
            Reason = errorReason;
            ExtendedReason = extenedReason;
        }

        public virtual ErrorReason Reason { get; protected set; }
        public virtual int ExtendedReason { get; protected set; }
    }
}
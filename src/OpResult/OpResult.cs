
namespace OpResult
{
    public class OpResult<TResult>
    {
        public bool WasSuccess { get; private set; }
        public TResult Result { get; private set; }
        public string NonSuccessMessage { get; private set; }
        public Exception Exception { get; private set; }

#pragma warning disable CS8618 // we use private ctor to avoid 
        private OpResult() { }
#pragma warning restore CS8618

        public static OpResult<TResult> Success(TResult result)
        {
            return new OpResult<TResult> { WasSuccess = true, Result = result };
        }

        public static OpResult<TResult> Failure(string error)
        {
            return new OpResult<TResult> { WasSuccess = false, NonSuccessMessage = error };
        }

        public static OpResult<TResult> Failure(TResult result, string error)
        {
            return new OpResult<TResult> { Result = result, WasSuccess = false, NonSuccessMessage = error };
        }

        public static OpResult<TResult> Failure(Exception ex, bool includeStackTrace = false)
        {
            var result = new OpResult<TResult>
            {
                WasSuccess = false,
                
                Exception = ex
            };
            if (includeStackTrace)
            {
                result.NonSuccessMessage = ex.Message;
            }
            else
            {
                result.NonSuccessMessage = String.Format("{0}{1}{1}{2}", ex.Message, Environment.NewLine, ex.StackTrace);
            }
            return result;
        }
    }
}


namespace FalconWare.ErrorHandling
{
    public class OpResultFactory
    {
        /// <summary>
        /// Creates a sucessful <see cref="OpResult{TResult}"/> with <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TResult">Type of result</typeparam>
        /// <param name="result">Result of the operation assigned to <see cref="OpResult{TResult}.Value"/>.</param>
        /// <returns>New unsuccessful <see cref="OpResult{TResult}.Value"/></returns>
        public static OpResult<TResult> CreateSuccess<TResult>(TResult result)
        {
            return new OpResult<TResult> { WasSuccess = true, Value = result };
        }

        /// <summary>
        /// Creates an unsucessful <see cref="OpResult{TResult}"/> with <paramref name="nonSuccessMessage"/>.
        /// </summary>
        /// <typeparam name="TResult">Type of result</typeparam>
        /// <param name="nonSuccessMessage">Human readable message summarising reason operation was not successful.</param>
        /// <returns>New unsuccessful <see cref="OpResult{TResult}.Result"/></returns>
        public static OpResult<TResult> CreateFailure<TResult>(string nonSuccessMessage)
        {
            return new OpResult<TResult> { WasSuccess = false, NonSuccessMessage = nonSuccessMessage };
        }

        /// <summary>
        /// Creates an unsucessful <see cref="OpResult{TResult}"/> with <paramref name="ex"/> setting `NonSuccessMessage` to `ex.Message`.
        /// </summary>
        /// <typeparam name="TResult">Type of result</typeparam>
        /// <param name="ex">`Exception` caught while performing the operation.</param>
        /// <param name="includeStackTrace">Whether to append the <paramref name="ex"/>'s strack trace on a newline to `NonSuccessMessage`, defaults to false.</param>
        /// <returns>New unsuccessful <see cref="OpResult{TResult}.Result"/></returns>
        public static OpResult<TResult> CreateFailure<TResult>(Exception ex, bool includeStackTrace = false)
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

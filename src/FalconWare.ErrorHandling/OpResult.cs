
namespace FalconWare.ErrorHandling
{
    /// <summary>
    /// Represents result of an operation successful or not. If successful <see cref="OpResult{TResult}.Result"/> is set; 
    /// otherwise <see cref="OpResult{TResult}.NonSuccessMessage"/> is set and <see cref="OpResult{TResult}.Exception"/> 
    /// may be set to an `Exception` that was caught when performing the operation.
    /// 
    /// Use <see cref="OpResultFactory"> to create an instance of this class.
    /// </summary>
    /// <typeparam name="TResult">Type of result</typeparam>
    public class OpResult<TResult>
    {
        /// <summary>
        /// Indicates whether the operation was successful.
        /// </summary>
        public bool WasSuccess { get; internal set; }

        /// <summary>
        /// Result of the operation only set if successful, e.g. `WasSuccess` is true.
        /// </summary>
        public TResult Result { get; internal set; }

        /// <summary>
        /// Human readable string why the operation was not succesful. Only set if the operation was not successful
        /// i.e. `WasSuccess` is false.
        /// </summary>
        public string NonSuccessMessage { get; internal set; }

        /// <summary>
        /// Exception that was caught when performing the operation which can be inspected for futher information as 
        /// to why operation failed. Only set if the operation was not successful i.e. `WasSuccess` is false.
        /// </summary>
        public Exception Exception { get; internal set; }

#pragma warning disable CS8618 // we use internal ctor to avoid the possibility of null props
        internal OpResult() { }
#pragma warning restore CS8618

    }
}

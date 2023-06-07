
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
        private bool _wasSuccess;
        private bool _wasSuccessChecked;
        private TResult _value;

        /// <summary>
        /// Indicates whether the operation was successful.
        /// </summary>
        public bool WasSuccess 
        { 
            get 
            {
                _wasSuccessChecked = true;
                return _wasSuccess;
            }
            internal set 
            {
                _wasSuccess = value;
            }
        }

        /// <summary>
        /// Result of the operation only set if successful, i.e. `WasSuccess` is true.
        ///
        /// Note `WasSuccess` MUST be checked before accessing this property - failure to do so will 
        /// result in if DEBUG symbol is set, i.e. in Debug builds.
        /// </summary>
        public TResult Value 
        {
             get 
             {
#if DEBUG
                if (!_wasSuccessChecked)
                {
                    throw new OpResultAccessException();
                }
#endif
                return _value;                
             }
             internal set 
             {
                _value = value;
             } 
        }

        /// <summary>
        /// Human readable string summarising why the operation was not succesful. Only set if the operation was not successful
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

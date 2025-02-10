using System;

namespace GHttpHelper.Axios.Interceptors
{
    public class InterceptorManager<T>
    {
        private Action<T>[] _handlers = Array.Empty<Action<T>>();
        private Action<Exception>[] _errorHandlers = Array.Empty<Action<Exception>>();

        public int Use(Action<T> fulfilled, Action<Exception> rejected = null)
        {
            var handlers = new Action<T>[_handlers.Length + 1];
            Array.Copy(_handlers, handlers, _handlers.Length);
            handlers[_handlers.Length] = fulfilled;
            _handlers = handlers;

            if (rejected != null)
            {
                var errorHandlers = new Action<Exception>[_errorHandlers.Length + 1];
                Array.Copy(_errorHandlers, errorHandlers, _errorHandlers.Length);
                errorHandlers[_errorHandlers.Length] = rejected;
                _errorHandlers = errorHandlers;
            }

            return _handlers.Length - 1;
        }

        public void Eject(int id)
        {
            if (id >= 0 && id < _handlers.Length)
            {
                var handlers = new Action<T>[_handlers.Length - 1];
                Array.Copy(_handlers, 0, handlers, 0, id);
                Array.Copy(_handlers, id + 1, handlers, id, _handlers.Length - id - 1);
                _handlers = handlers;

                if (id < _errorHandlers.Length)
                {
                    var errorHandlers = new Action<Exception>[_errorHandlers.Length - 1];
                    Array.Copy(_errorHandlers, 0, errorHandlers, 0, id);
                    Array.Copy(_errorHandlers, id + 1, errorHandlers, id, _errorHandlers.Length - id - 1);
                    _errorHandlers = errorHandlers;
                }
            }
        }

        internal void ExecuteHandlers(T value)
        {
            foreach (var handler in _handlers)
            {
                handler?.Invoke(value);
            }
        }

        internal void ExecuteErrorHandlers(Exception error)
        {
            foreach (var handler in _errorHandlers)
            {
                handler?.Invoke(error);
            }
        }
    }
}
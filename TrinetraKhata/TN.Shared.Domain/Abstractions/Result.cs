using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TN.Shared.Domain.Abstractions
{
    public class Result<T>
    {
        public bool IsSuccess {  get; set; }
        public IEnumerable<string>? Errors { get;}
        public T Data { get; set; }
        public string Message { get; set; }

        protected Result(bool isSuccess, IEnumerable<string> errors, T data = default!, string message = "")
        {
            IsSuccess = isSuccess;
            Errors = errors?? Enumerable.Empty<string>();
            Data = data;
            Message = message;
        }

        public static Result<T> Success(T data)
        {
            return new Result<T>(true, Enumerable.Empty<string>(), data);
        }

        public static Result<T> Success(string message)
        {
            return new Result<T>(true, Enumerable.Empty<string>(), default!, message);
        }

        public static Result<T> Failure(params string[] errors)
        {
            return new Result<T>(false, errors);

        }

        public static Result<T> Failure(string errors)
        {
            return new Result<T>(false, new List<string> { errors });
        }

       
       
    }
}

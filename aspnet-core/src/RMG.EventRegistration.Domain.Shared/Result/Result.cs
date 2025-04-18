﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.EventRegistration.Results
{
    public class Result
    {
        public bool Succeeded { get; init; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsFailed => !Succeeded;
        public bool Warning { get; init; }
        public bool IsWarned => this.Warning;
    }

    public class SucceededResult : Result
    {
        public string SuccessMessage { get; set; }
        public SucceededResult() : base()
        {
            Succeeded = true;
        }
    }

    public class Result<T>
    {
        public bool Succeeded { get; init; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsFailed => !Succeeded;
        public bool Warning { get; init; }
        public bool IsWarned => this.Warning;
        public T Value { get; set; }
    }

    public class ListResult<T> : Result<T>
    {
        public IEnumerable<T> Values { get; set; }
    }

    public class SucceededListResult<T> : ListResult<T> 
    {
        public SucceededListResult() : base()
        {
            Succeeded = true;
        }
    }

    public class FailedListResult<T> : ListResult<T>
    {
        public FailedListResult() : base()
        {
            Succeeded = false;
        }
    }

    public class SucceededResult<T> : Result<T>
    {
        public SucceededResult() : base()
        {
            Succeeded = true;
        }
    }

    public class FailedResult : Result
    {
        public FailedResult()
        {
            Succeeded = false;
        }

        public FailedResult(string errorCode, string errorMessage)
        {
            Succeeded = false;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }

    public class FailedResult<T> : Result<T>
    {
        public FailedResult() : base()
        {
            Succeeded = false;
        }

        public FailedResult(string errorCode, string errorMessage) : base()
        {
            Succeeded = false;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }

    public class FailedNullResult<T> : FailedResult<T>
    {
        public FailedNullResult(string objectName)
        {
            ErrorCode = $"{objectName}{ErrorContants.IsNullCode}";
            ErrorMessage = $"{objectName} {ErrorContants.IsNull}";
        }
    }

    public class FailedRequiredResult<T> : FailedResult<T>
    {
        public FailedRequiredResult(string objectName)
        {
            ErrorCode = $"{objectName}{ErrorContants.RequiredCode}";
            ErrorMessage = $"{objectName} {ErrorContants.Required}";
        }
    }

    public class FailedRequiredResult : FailedResult
    {
        public FailedRequiredResult(string objectName)
        {
            ErrorCode = $"{objectName}{ErrorContants.RequiredCode}";
            ErrorMessage = $"{objectName} {ErrorContants.Required}";
        }
    }


    public class FailedNullResult : FailedResult
    {
        public FailedNullResult(string objectName)
        {
            ErrorCode = $"{objectName}{ErrorContants.IsNullCode}";
            ErrorMessage = $"{objectName} {ErrorContants.IsNull}";
        }
    }

    public class FailedInvalidParamResult : FailedResult
    {
        public FailedInvalidParamResult(string objectName)
        {
            ErrorCode = $"{objectName}{ErrorContants.InvalidCode}";
            ErrorMessage = $"{objectName} {ErrorContants.Invalid}";
        }
    }

    public class FailedInvalidParamResult<T> : FailedResult<T>
    {
        public FailedInvalidParamResult(string objectName)
        {
            ErrorCode = $"{objectName}{ErrorContants.InvalidCode}";
            ErrorMessage = $"{objectName} {ErrorContants.Invalid}";
        }
    }

    public class WarningResult<T> : Result<T>
    {
        public WarningResult() : base()
        {
            Warning = true;
            Succeeded = false;
        }

        public WarningResult(string warningCode, string warningMessage) : base()
        {
            Warning = true;
            Succeeded = false;
            ErrorCode = warningCode;
            ErrorMessage = warningMessage;
        }
    }
}

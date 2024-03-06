using System.ComponentModel.DataAnnotations;
using Wolverine;

namespace Application.ExceptionInfos.Queries.GeExceptionInfoById;

public  record GetExceptionInfoByIdQuerie([Required] Guid Id) : IMessage;


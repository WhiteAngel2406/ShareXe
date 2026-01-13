namespace ShareXe.Base.Dtos
{
  public class SuccessResponse<T> : Response
  {
    public T? Data { get; set; }

    protected SuccessResponse()
    {
      Success = true;
    }

    public static SuccessResponse<T> WithData(T? data, string? message = null)
    {
      return new SuccessResponse<T>
      {
        Success = true,
        Data = data,
        Message = message
      };
    }


    public static SuccessResponse<T> WithMessage(string message)
    {
      return new SuccessResponse<T>
      {
        Success = true,
        Message = message
      };
    }

  }
}


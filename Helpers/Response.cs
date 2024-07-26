namespace WebApi.Helpers
{
    public class Response<T>
    {
        /// <summary>
        /// Status: false -> Error
        /// Status: true -> Ok
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// Code: 0 -> Error
        /// Code: 1 -> Ok
        /// </summary>
        public int Code { get; set; }
        public T Value { get; set; }

        public string Message { get; set; }

    }
}

namespace TwseApp.API.Models.Share
{
    /// <summary>
    /// 共用回傳格式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseModel<T>
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Status { get; set; } = false;

        /// <summary>
        /// 回傳訊息
        /// </summary>
        public string Message { get; set; } = null;

        /// <summary>
        /// 回傳資訊
        /// </summary>
        public T Data { get; set; } = default(T);

    }
}

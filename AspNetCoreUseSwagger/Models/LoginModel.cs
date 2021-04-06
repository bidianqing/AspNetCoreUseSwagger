using System.ComponentModel.DataAnnotations;

namespace AspNetCoreUseSwagger.Models
{
    /// <summary>
    /// 登陆模型类
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}

using Coin_Exchange.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coin_Exchange.Models.Modal
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { set; get; }
        public string fullName { set; get; }
        public string email { set; get; }
        public string password { set; get; }
        public bool IsEnable2FA { set; get; }
        public USER_ROLE role { set; get; } = USER_ROLE.ROLE_CUSTOMER;
    }
}

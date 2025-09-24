using System;

namespace MyEcomApi.Core.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string TokenHash { get; set; }    
        public string JwtId { get; set; }        
        public int UserId { get; set; }
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Revoked { get; set; }   
        public string ReplacedByToken { get; set; } 
    }
}

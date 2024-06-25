using APP.DAL.Repository.Entities;

namespace APP.Bus.Repository.DTOs.Product
{
    public class DTOImage
    {
        public int Code { get; set; }

        public string IdImage { get; set; } = null!;

        public string ImgUrl { get; set; } = null!;

        /// <summary>
        /// 0: FALSE
        /// 1: TRUE
        /// </summary>
        public bool IsThumbnail { get; set; } = false;
    }
}
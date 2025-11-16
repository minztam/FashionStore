using FashionStore.Models;

namespace FashionStore.DataSeed
{
    public static class DanhMucSeedData
    {
        public static IEnumerable<DanhMuc> GetSeedData()
        {
            return new List<DanhMuc>
            {
                new DanhMuc
                {
                    Ma_DanhMuc = "DM001",
                    Ten_DanhMuc = "Thời trang nam",
                    Ma_DanhMucCha = null,
                    Trang_Thai = true
                },
                new DanhMuc
                {
                    Ma_DanhMuc = "DM002",
                    Ten_DanhMuc = "Thời trang nữ",
                    Ma_DanhMucCha = null,
                    Trang_Thai = true
                },
                new DanhMuc
                {
                    Ma_DanhMuc = "DM001A",
                    Ten_DanhMuc = "Áo nam",
                    Ma_DanhMucCha = "DM001",
                    Trang_Thai = true
                },
                new DanhMuc
                {
                    Ma_DanhMuc = "DM001B",
                    Ten_DanhMuc = "Quần nam",
                    Ma_DanhMucCha = "DM001",
                    Trang_Thai = true
                },
                new DanhMuc
                {
                    Ma_DanhMuc = "DM002A",
                    Ten_DanhMuc = "Áo nữ",
                    Ma_DanhMucCha = "DM002",
                    Trang_Thai = true
                },
                new DanhMuc
                {
                    Ma_DanhMuc = "DM002B",
                    Ten_DanhMuc = "Váy nữ",
                    Ma_DanhMucCha = "DM002",
                    Trang_Thai = true
                }
            };
        }
    }
}

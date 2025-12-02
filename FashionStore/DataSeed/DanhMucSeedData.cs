using FashionStore.Models;

namespace FashionStore.DataSeed
{
    public static class DanhMucSeedData
    {
        public static IEnumerable<DanhMuc> GetSeedData()
        {
            return new List<DanhMuc>
            {
                // Danh mục cha
                new DanhMuc { Ma_DanhMuc = "DM001", Ten_DanhMuc = "Thời trang nam", Ma_DanhMucCha = null, Trang_Thai = true },
                new DanhMuc { Ma_DanhMuc = "DM002", Ten_DanhMuc = "Thời trang nữ", Ma_DanhMucCha = null, Trang_Thai = true },

                // Danh mục con NAM
                new DanhMuc { Ma_DanhMuc = "DM001A", Ten_DanhMuc = "Áo thun & Polo", Ma_DanhMucCha = "DM001", Trang_Thai = true },
                new DanhMuc { Ma_DanhMuc = "DM001B", Ten_DanhMuc = "Áo sơ mi nam", Ma_DanhMucCha = "DM001", Trang_Thai = true },
                new DanhMuc { Ma_DanhMuc = "DM001C", Ten_DanhMuc = "Áo khoác & Hoodie nam", Ma_DanhMucCha = "DM001", Trang_Thai = true },
                new DanhMuc { Ma_DanhMuc = "DM001D", Ten_DanhMuc = "Quần jeans nam", Ma_DanhMucCha = "DM001", Trang_Thai = true },
                new DanhMuc { Ma_DanhMuc = "DM001E", Ten_DanhMuc = "Quần short & Jogger nam", Ma_DanhMucCha = "DM001", Trang_Thai = true },

                // Danh mục con NỮ
                new DanhMuc { Ma_DanhMuc = "DM002A", Ten_DanhMuc = "Áo nữ & Croptop", Ma_DanhMucCha = "DM002", Trang_Thai = true },
                new DanhMuc { Ma_DanhMuc = "DM002B", Ten_DanhMuc = "Váy & Đầm nữ", Ma_DanhMucCha = "DM002", Trang_Thai = true },
                new DanhMuc { Ma_DanhMuc = "DM002C", Ten_DanhMuc = "Quần nữ & Legging", Ma_DanhMucCha = "DM002", Trang_Thai = true },
                new DanhMuc { Ma_DanhMuc = "DM002D", Ten_DanhMuc = "Áo khoác nữ", Ma_DanhMucCha = "DM002", Trang_Thai = true }
            };
        }
    }
}

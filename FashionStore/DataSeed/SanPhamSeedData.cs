using FashionStore.Models;

namespace FashionStore.DataSeed
{
    public static class SanPhamSeedData
    {
        public static IEnumerable<SanPham> GetSeedData()
        {
            return new List<SanPham>
            {
                // ===================== SẢN PHẨM NAM =====================
                new SanPham
                {
                    Ma_SanPham = "SP001",
                    Ten_SanPham = "Áo Thun Trơn Basic Unisex 100% Cotton 4 Chiều",
                    Ma_DanhMuc = "DM001A",
                    Mo_Ta =
@"Form rộng chuẩn street style, vải dày dặn không xù lông, mặc cực mát
Bảng size:
Size M:  Cao < 1m60, Nặng < 60kg
Size L:  Cao < 1m70, Nặng < 68kg
Size XL: Cao < 1m75, Nặng < 75kg
Size XXL: Cao < 1m80, Nặng < 85kg",
                    Trang_Thai = true
                },

                new SanPham
                {
                    Ma_SanPham = "SP002",
                    Ten_SanPham = "Áo Polo Nam Cao Cấp Pima Cotton Form Chuẩn",
                    Ma_DanhMuc = "DM001A",
                    Mo_Ta =
@"Vải Pima cotton siêu mềm, cổ bẻ đứng form, logo thêu nổi bật
Bảng size:
Size M:  Cao 1m55–1m65, Nặng 50–60kg
Size L:  Cao 1m65–1m72, Nặng 60–70kg
Size XL: Cao 1m72–1m78, Nặng 70–78kg
Size XXL: Cao 1m78–1m82, Nặng 78–85kg",
                    Trang_Thai = true
                },

                new SanPham
                {
                    Ma_SanPham = "SP003",
                    Ten_SanPham = "Sơ Mi Nam Oxford Dài Tay Cao Cấp Form Chuẩn",
                    Ma_DanhMuc = "DM001B",
                    Mo_Ta =
@"Vải cotton thoáng mát, form chuẩn công sở, không nhăn
Bảng size:
Size M:  Cao 1m55–1m65, Nặng 48–60kg
Size L:  Cao 1m65–1m72, Nặng 60–68kg
Size XL: Cao 1m72–1m78, Nặng 68–75kg
Size XXL: Cao 1m78–1m85, Nặng 75–85kg",
                    Trang_Thai = true
                },

                new SanPham
                {
                    Ma_SanPham = "SP004",
                    Ten_SanPham = "Áo Hoodie Unisex Form Rộng Nỉ Bông 380GSM",
                    Ma_DanhMuc = "DM001C",
                    Mo_Ta =
@"Nỉ chân cua dày dặn, ấm áp mùa đông, form rộng cực chất
Bảng size:
Size M:  Cao < 1m60, Nặng < 60kg
Size L:  Cao < 1m68, Nặng < 68kg
Size XL: Cao < 1m75, Nặng < 78kg
Size XXL: Cao < 1m82, Nặng < 90kg",
                    Trang_Thai = true
                },

                new SanPham
                {
                    Ma_SanPham = "SP005",
                    Ten_SanPham = "Quần Jeans Nam Slimfit Co Giãn 4 Chiều",
                    Ma_DanhMuc = "DM001D",
                    Mo_Ta =
@"Jeans cao cấp co giãn 4 chiều, form ôm tôn dáng, bền đẹp
Bảng size:
Size M:  Cao < 1m60, Nặng < 60kg
Size L:  Cao < 1m65, Nặng < 70kg
Size XL: Cao < 1m70, Nặng < 76kg
Size XXL: Cao < 1m75, Nặng < 80kg",
                    Trang_Thai = true
                },

                new SanPham
                {
                    Ma_SanPham = "SP006",
                    Ten_SanPham = "Quần Short Kaki Nam Ống Rộng Cargo 6 Túi",
                    Ma_DanhMuc = "DM001E",
                    Mo_Ta =
@"Hot trend 2025, nhiều túi tiện lợi, chất kaki dày dặn
Bảng size:
Size M:  Eo 70–75cm
Size L:  Eo 76–82cm
Size XL: Eo 83–90cm
Size XXL: Eo 90–100cm",
                    Trang_Thai = true
                },

                new SanPham
                {
                    Ma_SanPham = "SP023",
                    Ten_SanPham = "Quần Jogger Nam Thun Bo Gấu Co Giãn",
                    Ma_DanhMuc = "DM001E",
                    Mo_Ta =
@"Mặc nhà hoặc thể thao đều thoải mái, form đẹp
Bảng size:
Size M:  Cao 1m55–1m65, Nặng 48–60kg
Size L:  Cao 1m65–1m72, Nặng 60–70kg
Size XL: Cao 1m72–1m78, Nặng 70–78kg
Size XXL: Cao 1m78–1m85, Nặng 78–90kg",
                    Trang_Thai = true
                },


                // ===================== SẢN PHẨM NỮ =====================
                new SanPham
                {
                    Ma_SanPham = "SP007",
                    Ten_SanPham = "Áo Croptop Nữ Tay Lỡ Form Rộng Cotton",
                    Ma_DanhMuc = "DM002A",
                    Mo_Ta =
@"Oversize cực xinh, mix đồ nào cũng đẹp, vải cotton mềm mại
Bảng size:
Size S:  Cao < 1m55, Nặng < 48kg
Size M:  Cao < 1m60, Nặng < 55kg
Size L:  Cao < 1m65, Nặng < 62kg
Size XL: Cao < 1m70, Nặng < 70kg",
                    Trang_Thai = true
                },

                new SanPham
                {
                    Ma_SanPham = "SP022",
                    Ten_SanPham = "Áo Sơ Mi Nữ Form Rộng Voan Lụa Cao Cấp",
                    Ma_DanhMuc = "DM002A",
                    Mo_Ta =
@"Mặc đi làm đi chơi đều sang chảnh, chất voan lụa mát
Bảng size:
Size S:  Cao 1m50–1m58, Nặng 40–48kg
Size M:  Cao 1m58–1m62, Nặng 48–55kg
Size L:  Cao 1m62–1m68, Nặng 55–62kg
Size XL: Cao 1m68–1m72, Nặng 62–70kg",
                    Trang_Thai = true
                },

                new SanPham
                {
                    Ma_SanPham = "SP008",
                    Ten_SanPham = "Váy Đầm Nữ Dáng A Babydoll Xinh Xắn",
                    Ma_DanhMuc = "DM002B",
                    Mo_Ta =
@"Che bụng mỡ cực tốt, mặc đi làm đi chơi đều hợp, freesize
Bảng size:
Freesize: phù hợp Cao 1m50–1m68, Nặng 40–60kg",
                    Trang_Thai = true
                },

                new SanPham
                {
                    Ma_SanPham = "SP009",
                    Ten_SanPham = "Quần Legging Nữ Lưng Cao Độn Mông",
                    Ma_DanhMuc = "DM002C",
                    Mo_Ta =
@"Vải cotton dày dặn, nâng mông tự nhiên, tôn dáng cực đẹp
Bảng size:
Size S:  Eo 55–62cm
Size M:  Eo 63–70cm
Size L:  Eo 71–78cm
Size XL: Eo 79–88cm",
                    Trang_Thai = true
                },

                new SanPham
                {
                    Ma_SanPham = "SP010",
                    Ten_SanPham = "Áo Khoác Gió Nữ 2 Lớp Chống Nước Có Mũ",
                    Ma_DanhMuc = "DM002D",
                    Mo_Ta =
@"Form rộng đẹp, đi mưa nhẹ thoải mái, chống gió lạnh tốt
Bảng size:
Size M:  Cao 1m50–1m58, Nặng 40–52kg
Size L:  Cao 1m58–1m65, Nặng 52–60kg
Size XL: Cao 1m65–1m72, Nặng 60–70kg
Size XXL: Cao 1m70–1m75, Nặng 70–78kg",
                    Trang_Thai = true
                }
            };
        }
    }
}

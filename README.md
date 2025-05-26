Báo cáo Đồ án: Nền tảng Giao dịch Tiền điện tử (Crypto Trading Platform)

* Thông tin Sinh viên Thực hiện

- Sinh viên 1:
    + Họ và Tên: Võ Thành Nhân
    + Mã Sinh viên: 22115053122234
- Sinh viên 2:
    + Họ và Tên: Huỳnh Nguyên Vĩnh Nguyên
    + Mã Sinh viên: 22115053122233

1. Giới thiệu Dự án

- Dự án "Nền tảng Giao dịch Tiền điện tử" được xây dựng nhằm mục đích tạo ra một môi trường mô phỏng cho người dùng để tìm hiểu, theo dõi và thực hiện các giao dịch tiền điện tử. Hệ thống bao gồm các chức năng cho người dùng thông thường như xem thị trường, quản lý danh mục đầu tư, watchlist, ví tiền, cũng như các chức năng quản trị cho admin.

2. Nội dung Công việc Dự kiến

Dự án được triển khai với các hạng mục công việc chính dự kiến như sau:

2.1. Frontend (Giao diện Người dùng - Reactjs)
* Các trang chính:
    * Trang chủ: Hiển thị thông tin thị trường chung, danh sách coin.
    * Trang Chi tiết Coin: Biểu đồ giá, mua/bán coin.
    * Trang Đăng nhập / Đăng ký.
    * Trang Hồ sơ Người dùng (Profile): Quản lý thông tin cá nhân, xác thực hai yếu tố (2FA).
    * Trang Watchlist: Theo dõi các coin yêu thích.
    * Trang Portfolio: Hiển thị tài sản nắm giữ.
    * Trang Ví (Wallet): Xem số dư, lịch sử giao dịch, nạp tiền, rút tiền , chuyển tiền cho account khác.
    * Trang Thông tin Thanh toán (Payment Details).
    * Trang Rút tiền (Withdrawal).
    * Trang Lịch sử Hoạt động.
    * Trang Admin: Quản lý yêu cầu rút tiền.
    * Chatbot hỗ trợ hỏi đáp về coin.

2.2. Backend (Hệ thống Xử lý - ASP.NET Core)
* Xác thực và phân quyền người dùng bằng JWT.
* Triển khai logic nghiệp vụ cho việc gửi/nhận OTP, xác thực 2FA.
* Tương tác với cơ sở dữ liệu SQLServer để lưu trữ và truy xuất thông tin người dùng, coin, watchlist, ví, giao dịch.
* Xử lý logic thêm/xóa coin khỏi watchlist.
* Xử lý logic cho các yêu cầu rút tiền (tạo yêu cầu, admin xử lý).
* API cho chatbot.

2.3. Cơ sở dữ liệu
* Thiết kế và triển khai CSDL quan hệ (SQLServer) để lưu trữ thông tin.
* Đảm bảo tính toàn vẹn và bảo mật dữ liệu.

3. Kết quả Đạt được

3.1. Chức năng Người dùng (User)
* Xác thực:
    - Đăng ký, Đăng nhập người dùng.
    - Xác thực hai yếu tố (2FA): Gửi và xác thực OTP.
    - Lấy thông tin người dùng đã đăng nhập (JWT).
* Thị trường & Coin:
    - Hiển thị danh sách tiền điện tử (trang Home).
    - Xem chi tiết một loại tiền điện tử (trang StockDetails, hiển thị thông tin, biểu đồ giá).
* Quản lý Watchlist:
    - Hiển thị watchlist của người dùng.
    - Thêm/Xóa coin khỏi watchlist (đã có code frontend và backend API).
* Quản lý Ví & Giao dịch:
    - Hiển thị trang Ví.
    - Quản lý Thông tin Thanh toán (Payment Details): Form thêm/sửa, hiển thị thông tin.
    - Thực hiện Yêu cầu Rút tiền (Withdrawal).
* Thông tin Tài khoản:
    - Hiển thị trang Hồ sơ Người dùng.
    - Xem Lịch sử Hoạt động.
    - Hiển thị trang Portfolio.
* Chatbot: Đã có khung giao diện Chatbot ở trang Home, có khả năng gửi và nhận tin nhắn .

3.2. Chức năng Quản trị viên (Admin)
* Quản lý Rút tiền:
    - Hiển thị danh sách các yêu cầu rút tiền đang chờ xử lý (`WithdrawalAdmin` page).
    - Chấp nhận hoặc Từ chối yêu cầu rút tiền (đã có code frontend và backend API).


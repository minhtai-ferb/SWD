import 'package:flutter/material.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'collection_page.dart'; // Import trang CollectionPage
import 'practice_page.dart'; // Import trang PracticePage
import 'login_page.dart'; // Import trang LoginPage

class HomePage extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    User? user = FirebaseAuth.instance.currentUser;

    return Scaffold(
      appBar: AppBar(
        title: Text('Từ Điển Tiếng Nhật'),
        backgroundColor: Colors.blueAccent,
        actions: [
          user == null
              ? TextButton(
            onPressed: () {
              Navigator.push(
                context,
                MaterialPageRoute(builder: (context) => LoginPage()),
              );
            },
            child: Text(
              'Login',
              style: TextStyle(color: Colors.white),
            ),
          )
              : TextButton(
            onPressed: () async {
              await FirebaseAuth.instance.signOut();
              ScaffoldMessenger.of(context).showSnackBar(
                SnackBar(content: Text('Đã đăng xuất.')),
              );
              // Sau khi đăng xuất, reload lại trang để cập nhật trạng thái
              Navigator.pushReplacement(
                context,
                MaterialPageRoute(builder: (context) => HomePage()),
              );
            },
            child: Text(
              'Logout',
              style: TextStyle(color: Colors.white),
            ),
          ),
        ],
      ),
      body: Padding(
        padding: EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // Hiển thị thông tin người dùng
            user != null
                ? Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  'Xin chào, ${user.displayName}',
                  style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
                ),
                SizedBox(height: 10),
                Text('Email: ${user.email}'),
                SizedBox(height: 20),
              ],
            )
                : Container(),
            // Ô tìm kiếm và danh sách từ vựng
            TextField(
              decoration: InputDecoration(
                labelText: 'Nhập từ bạn muốn tìm kiếm...',
                prefixIcon: Icon(Icons.search),
                border: OutlineInputBorder(),
              ),
            ),
            SizedBox(height: 20),
            Text(
              'Từ vựng phổ biến',
              style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
            ),
            SizedBox(height: 10),
            Expanded(
              child: ListView.builder(
                itemCount: 10, // số lượng từ vựng mẫu
                itemBuilder: (context, index) {
                  return Card(
                    child: ListTile(
                      leading: Icon(Icons.book),
                      title: Text('Từ vựng $index'),
                      subtitle: Text('Nghĩa của từ vựng $index'),
                    ),
                  );
                },
              ),
            ),
          ],
        ),
      ),
      bottomNavigationBar: BottomNavigationBar(
        items: [
          BottomNavigationBarItem(
            icon: Icon(Icons.home),
            label: 'Home',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.library_books),
            label: 'Collection',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.school),
            label: 'Practice',
          ),
        ],
        onTap: (index) {
          switch (index) {
            case 0:
            // Khi nhấn nút Home
              Navigator.pushReplacement(
                context,
                MaterialPageRoute(builder: (context) => HomePage()),
              );
              break;
            case 1:
            // Khi nhấn nút Collection
              Navigator.pushReplacement(
                context,
                MaterialPageRoute(builder: (context) => CollectionPage()),
              );
              break;
            case 2:
            // Khi nhấn nút Practice
              Navigator.pushReplacement(
                context,
                MaterialPageRoute(builder: (context) => PracticePage()),
              );
              break;
          }
        },
      ),
    );
  }
}

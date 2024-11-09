import 'package:flutter/material.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'package:google_sign_in/google_sign_in.dart';
import 'package:http/http.dart' as http; // Thêm thư viện http để gọi API
import 'dart:convert'; // Để xử lý JSON

import 'home_page.dart';

class GoogleSignInPage extends StatefulWidget {
  @override
  _GoogleSignInPageState createState() => _GoogleSignInPageState();
}

class _GoogleSignInPageState extends State<GoogleSignInPage> {
  final FirebaseAuth _auth = FirebaseAuth.instance;
  final GoogleSignIn _googleSignIn = GoogleSignIn();

  Future<void> _signInWithGoogle() async {
    try {
      // Bước 1: Đăng nhập với Google
      final GoogleSignInAccount? googleUser = await _googleSignIn.signIn();
      if (googleUser == null) return; // Người dùng đã hủy đăng nhập

      final GoogleSignInAuthentication googleAuth = await googleUser.authentication;

      // Bước 2: Tạo một đối tượng credential từ thông tin đăng nhập của Google
      final credential = GoogleAuthProvider.credential(
        accessToken: googleAuth.accessToken,
        idToken: googleAuth.idToken,
      );

      // Bước 3: Đăng nhập vào Firebase với credential
      await _auth.signInWithCredential(credential);

      // Bước 4: Gọi API với access token
      if (googleAuth.accessToken != null) {
        await callApiWithAccessToken(googleAuth.accessToken!);
      }

      // Chuyển đến trang HomePage sau khi đăng nhập thành công
      Navigator.pushReplacement(
        context,
        MaterialPageRoute(builder: (context) => HomePage()),
      );
    } catch (error) {
      print("Đã xảy ra lỗi: $error");
      // Xử lý lỗi tại đây
    }
  }

  Future<void> callApiWithAccessToken(String accessToken) async {
    const apiUrl = 'https://your-api-endpoint.com/endpoint'; // Đổi URL thành API của bạn

    try {
      final response = await http.post(
        Uri.parse(apiUrl),
        headers: {
          'Content-Type': 'application/json',
          'Authorization': 'Bearer $accessToken',
        },
        body: jsonEncode({"accessToken": accessToken}),
      );

      if (response.statusCode == 200) {
        print("API gọi thành công: ${response.body}");
      } else {
        print("Lỗi khi gọi API: ${response.statusCode} - ${response.body}");
      }
    } catch (e) {
      print("Đã xảy ra lỗi khi gọi API: $e");
    }
  }

  Future<void> _signOut() async {
    await _auth.signOut(); // Đăng xuất khỏi Firebase
    await _googleSignIn.signOut(); // Đăng xuất khỏi Google
    // Sau khi đăng xuất, có thể điều hướng đến trang login hoặc homepage
    Navigator.pop(context); // Trở lại trang trước
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Đăng Nhập bằng Google'),
        leading: IconButton(
          icon: Icon(Icons.arrow_back),
          onPressed: () {
            Navigator.pop(context); // Quay lại trang trước
          },
        ),
      ),
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            ElevatedButton(
              onPressed: _signInWithGoogle,
              child: Text('Đăng Nhập với Google'),
            ),
            SizedBox(height: 20),
            ElevatedButton(
              onPressed: _signOut,
              child: Text('Đăng Xuất'),
            ),
          ],
        ),
      ),
    );
  }
}

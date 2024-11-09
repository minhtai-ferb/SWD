import 'package:flutter/material.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'google_signin_page.dart';
import 'home_page.dart';
import 'register_page.dart';
import 'forgot_password_page.dart';

class LoginPage extends StatefulWidget {
  @override
  _LoginPageState createState() => _LoginPageState();
}

class _LoginPageState extends State<LoginPage> {
  final TextEditingController _emailController = TextEditingController();
  final TextEditingController _passwordController = TextEditingController();
  final FirebaseAuth _auth = FirebaseAuth.instance;

  String? _errorMessage;

  bool _isPasswordValid(String password) {
    // Kiểm tra mật khẩu có chứa ký tự đặc biệt và độ dài
    final RegExp passwordRegExp = RegExp(r'^(?=.*?[!@#$%^&*])[A-Za-z\d!@#$%^&*]{6,12}$');
    return passwordRegExp.hasMatch(password);
  }

  bool _isEmailValid(String email) {
    // Kiểm tra định dạng email
    final RegExp emailRegExp = RegExp(r'^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$');
    return emailRegExp.hasMatch(email);
  }

  Future<void> _login() async {
    String email = _emailController.text.trim();
    String password = _passwordController.text.trim();

    setState(() {
      _errorMessage = null; // Reset lỗi trước khi kiểm tra
    });

    // Kiểm tra định dạng email
    if (!_isEmailValid(email)) {
      setState(() {
        _errorMessage = 'Email không hợp lệ. Vui lòng kiểm tra lại.';
      });
      return;
    }

    // Kiểm tra định dạng mật khẩu
    if (!_isPasswordValid(password)) {
      setState(() {
        _errorMessage = 'Mật khẩu phải chứa ít nhất một ký tự đặc biệt và có độ dài từ 6 đến 12 ký tự.';
      });
      return;
    }

    try {
      // Đăng nhập với email và password
      UserCredential userCredential = await _auth.signInWithEmailAndPassword(email: email, password: password);
      // Chuyển đến trang HomePage nếu thành công
      Navigator.pushReplacement(
        context,
        MaterialPageRoute(builder: (context) => HomePage()),
      );
    } on FirebaseAuthException catch (e) {
      setState(() {
        if (e.code == 'user-not-found') {
          _errorMessage = 'Tài khoản không tồn tại. Vui lòng kiểm tra lại.';
        } else if (e.code == 'wrong-password') {
          _errorMessage = 'Mật khẩu không chính xác.';
        } else {
          _errorMessage = 'Đã xảy ra lỗi. Vui lòng thử lại.';
        }
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Đăng Nhập'),
        leading: IconButton(
          icon: Icon(Icons.arrow_back),
          onPressed: () {
            Navigator.pop(context); // Quay lại trang trước
          },
        ),
      ),
      body: Padding(
        padding: EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            TextField(
              controller: _emailController,
              decoration: InputDecoration(
                labelText: 'Email',
                border: OutlineInputBorder(),
              ),
            ),
            SizedBox(height: 16),
            TextField(
              controller: _passwordController,
              obscureText: true,
              decoration: InputDecoration(
                labelText: 'Mật Khẩu',
                border: OutlineInputBorder(),
              ),
            ),
            SizedBox(height: 16),
            Align(
              alignment: Alignment.centerRight,
              child: TextButton(
                child: Text('Quên mật khẩu?'),
                onPressed: () {
                  Navigator.push(
                    context,
                    MaterialPageRoute(builder: (context) => ForgotPasswordPage()),
                  );
                },
              ),
            ),
            SizedBox(height: 16),
            if (_errorMessage != null)
              Text(
                _errorMessage!,
                style: TextStyle(color: Colors.red),
              ),
            SizedBox(height: 16),
            ElevatedButton(
              onPressed: _login,
              child: Text('Login'),
            ),
            SizedBox(height: 16),
            Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                ElevatedButton.icon(
                  icon: Icon(Icons.g_mobiledata),
                  label: Text('Google'),
                  onPressed: () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(builder: (context) => GoogleSignInPage()),
                    );
                  },
                ),
                SizedBox(width: 10),
                ElevatedButton(
                  onPressed: () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(builder: (context) => RegisterPage()),
                    );
                  },
                  child: Text('Register'),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }
}

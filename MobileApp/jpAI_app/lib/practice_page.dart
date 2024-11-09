import 'package:flutter/material.dart';
import 'home_page.dart';
import 'chat_bot_page.dart';
import 'payment_page.dart'; // Import PaymentPage để điều hướng đến khi nhấn nút PayPal

class PracticePage extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Bài Tập Từ Vựng'),
        leading: IconButton(
          icon: Icon(Icons.arrow_back),
          onPressed: () {
            Navigator.pushReplacement(
              context,
              MaterialPageRoute(builder: (context) => HomePage()),
            );
          },
        ),
      ),
      body: Padding(
        padding: EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              'Bài Tập Từ Vựng:',
              style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
            ),
            SizedBox(height: 16),
            Expanded(
              child: ListView(
                children: [
                  ListTile(
                    title: Text('Bài tập 1'),
                    subtitle: Text('Chi tiết bài tập 1'),
                  ),
                  ListTile(
                    title: Text('Bài tập 2'),
                    subtitle: Text('Chi tiết bài tập 2'),
                  ),
                ],
              ),
            ),
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceAround,
              children: [
                ElevatedButton(
                  onPressed: () {
                    // Điều hướng đến trang Chat Bot
                    Navigator.push(
                      context,
                      MaterialPageRoute(builder: (context) => MessagesPage()),
                    );
                  },
                  child: Text('Chat Bot'),
                ),
                ElevatedButton(
                  onPressed: () {
                    // Điều hướng đến trang Payment
                    Navigator.push(
                      context,
                      MaterialPageRoute(builder: (context) => PaymentScreen()),
                    );
                  },
                  child: Text('PayPal'),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }
}

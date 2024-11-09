import 'package:flutter/material.dart';
import 'home_page.dart'; // Import HomePage để quay về khi nhấn Back

class CollectionPage extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Từ Vựng Yêu Thích'),
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
              'Danh Sách Từ Vựng Yêu Thích:',
              style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
            ),
            SizedBox(height: 16),
            Expanded(
              child: ListView(
                children: [
                  ListTile(
                    title: Text('Từ vựng 1'),
                    subtitle: Text('Nghĩa của từ 1'),
                  ),
                  ListTile(
                    title: Text('Từ vựng 2'),
                    subtitle: Text('Nghĩa của từ 2'),
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}

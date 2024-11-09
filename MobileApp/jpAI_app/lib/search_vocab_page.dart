import 'package:flutter/material.dart';

class SearchVocabPage extends StatelessWidget {
  final String vocabWord; // Từ vựng tìm kiếm
  final String pronunciation; // Phát âm
  final String meaning; // Ngữ nghĩa
  final String example; // Ví dụ

  // Constructor để nhận dữ liệu từ HomePage hoặc một trang khác
  SearchVocabPage({
    required this.vocabWord,
    required this.pronunciation,
    required this.meaning,
    required this.example,
  });

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Tìm Kiếm Từ Vựng'),
        leading: IconButton(
          icon: Icon(Icons.arrow_back),
          onPressed: () {
            Navigator.pop(context); // Quay lại trang trước
          },
        ),
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              'Từ Vựng: $vocabWord',
              style: TextStyle(fontSize: 24, fontWeight: FontWeight.bold),
            ),
            SizedBox(height: 8),
            Text(
              'Phát Âm: $pronunciation',
              style: TextStyle(fontSize: 18),
            ),
            SizedBox(height: 8),
            Text(
              'Ngữ Nghĩa: $meaning',
              style: TextStyle(fontSize: 18),
            ),
            SizedBox(height: 8),
            Text(
              'Ví Dụ: $example',
              style: TextStyle(fontSize: 18),
            ),
          ],
        ),
      ),
    );
  }
}

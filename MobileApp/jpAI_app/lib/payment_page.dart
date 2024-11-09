import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:url_launcher/url_launcher.dart';

class PaymentScreen extends StatefulWidget {
  @override
  _PaymentScreenState createState() => _PaymentScreenState();
}

class _PaymentScreenState extends State<PaymentScreen> {
  Future<void> _makePayment() async {
    try {
      final response = await http.post(
        Uri.parse('https://localhost:7217/api/packages'),
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({}),
      );
      if (response.statusCode == 200) {
        final paymentData = json.decode(response.body);
        final approvalUrl = paymentData['approvalUrl'];

        if (await canLaunch(approvalUrl)) {
          await launch(approvalUrl);
        } else {
          throw 'Could not launch $approvalUrl';
        }
      } else {
        throw Exception('Failed to create Paypal order');
      }
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Payment initiation failed: $e')),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text('Flutter PayPal Payment')),
      body: Center(
        child: ElevatedButton(
          onPressed: _makePayment,
          child: Text('Pay 9.99\$'),
        ),
      ),
    );
  }
}

'use client';

import { completeSubscription } from '@/actions/payment-subscription';
import { useSearchParams, useRouter } from 'next/navigation';
import { useState, useEffect } from 'react'; // Đường dẫn tới file chứa hàm completeSubscription

export default function SuccessPage() {
  const searchParams = useSearchParams();
  const router = useRouter();

  // Lấy các tham số từ URL
  const paymentId = searchParams.get('paymentId');
  const payerId = searchParams.get('PayerID');
  const token = searchParams.get('token');

  // State để lưu số giây còn lại
  const [countdown, setCountdown] = useState(0);
  const [isCompleted, setIsCompleted] = useState(false);

  useEffect(() => {
    if (paymentId && payerId && token) {
      const complete = async () => {
        try {
          const response = await completeSubscription(paymentId, payerId, token);
          console.log("Subscription completed response:", response);

          // Nếu API thành công, bắt đầu đếm ngược
          setIsCompleted(true);
          setCountdown(5);
        } catch (error) {
          console.error("Failed to complete subscription:", error);
        }
      };

      complete();
    }
  }, [paymentId, payerId, token]);

  useEffect(() => {
    if (isCompleted && countdown > 0) {
      const timer = setInterval(() => {
        setCountdown(prev => {
          if (prev === 1) {
            clearInterval(timer);
            router.push('/');
          }
          return prev - 1;
        });
      }, 1000);
    }
  }, [isCompleted, countdown, router]);

  return (
    <div>
      <h1>Payment Success</h1>
      <p>Payment ID: {paymentId}</p>
      <p>Payer ID: {payerId}</p>
      {isCompleted ? (
        <p>Redirecting to homepage in {countdown} seconds...</p>
      ) : (
        <p>Processing payment...</p>
      )}
    </div>
  );
}

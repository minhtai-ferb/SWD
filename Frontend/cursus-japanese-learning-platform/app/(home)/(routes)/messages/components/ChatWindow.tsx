import { createMessages, getAllMessagesByChatId } from "@/actions/messages";
import React, { useState, useEffect } from "react";

type Message = {
    id: string;
    messageContent: string;
    senderType: string;
};

type ChatWindowProps = {
    chatId: string;
};

const ChatWindow: React.FC<ChatWindowProps> = ({ chatId }) => {
    const [messages, setMessages] = useState<Message[]>([]);
    const [newMessage, setNewMessage] = useState("");
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        const fetchMessages = async () => {
            try {
                const result = await getAllMessagesByChatId(chatId);
                if (Array.isArray(result.data)) {
                    setMessages(result.data);
                } else {
                    console.error("[ChatWindow] Result data is not an array:", result.data);
                    setMessages([]);
                }
            } catch (error) {
                console.error("[ChatWindow] Error fetching messages", error);
            }
        };

        if (chatId) {
            fetchMessages();
        }
    }, [chatId]);



const handleSendMessage = async () => {
    if (newMessage.trim() === "") return;

    setLoading(true);

    // Gửi tin nhắn của người dùng ngay lập tức
    const userMessage = {
        id: `user-${Date.now()}`,
        messageContent: newMessage,
        senderType: "User",
    };

    // Cập nhật tin nhắn người dùng vào state ngay lập tức
    setMessages((prevMessages) => [...prevMessages, userMessage]);

    try {
        // Lưu tin nhắn của người dùng vào cơ sở dữ liệu
        const userMessageResponse = await createMessages(chatId, newMessage, "User");
        if (userMessageResponse && userMessageResponse.id) {
            // Cập nhật lại tin nhắn người dùng khi có phản hồi từ server
            setMessages((prevMessages) => prevMessages.map(msg =>
                msg.id === userMessage.id ? userMessageResponse : msg
            ));
        }

        // Gửi tin nhắn bot ngay lập tức
        const botMessage = {
            id: `bot-${Date.now()}`,
            messageContent: "...", // Placeholder message, sẽ cập nhật sau
            senderType: "Bot",
        };

        // Cập nhật tin nhắn Bot vào state ngay lập tức
        setMessages((prevMessages) => [...prevMessages, botMessage]);

        // Lấy phản hồi từ chatbot API
        const botMessageResponse = await getChatbotResponse(newMessage);
        if (botMessageResponse) {
            // Cập nhật tin nhắn Bot sau khi nhận phản hồi
            const updatedBotMessage = { ...botMessage, messageContent: botMessageResponse };

            // Cập nhật lại tin nhắn của Bot trong state để hiển thị ngay lập tức
            setMessages((prevMessages) => prevMessages.map(msg =>
                msg.id === botMessage.id ? updatedBotMessage : msg
            ));

            // Lưu tin nhắn của bot vào cơ sở dữ liệu
            const botMessageSaved = await createMessages(chatId, botMessageResponse, "Bot");
            if (botMessageSaved && botMessageSaved.id) {
                // Cập nhật lại tin nhắn Bot khi có phản hồi từ server
                setMessages((prevMessages) => prevMessages.map(msg =>
                    msg.id === updatedBotMessage.id ? botMessageSaved : msg
                ));
            }
        }
    } catch (error) {
        console.error("[ChatWindow] Error sending message", error);
    } finally {
        setLoading(false);
        setNewMessage(""); // Reset the input field after sending the message
    }
};


    const getChatbotResponse = async (message: string) => {
        try {
            const apiKey = "AIzaSyBpwEbwSuVb1Aod6FNnoPVVOh53oRFZ3wU";  // Replace with actual API key
            const response = await fetch(`https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-pro-002:generateContent?key=${apiKey}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    contents: [{
                        parts: [{
                            text: `User message: "${message}"\n\nPlease respond in Japanese language only. Make sure to use natural Japanese expressions and appropriate levels of formality.`
                        }]
                    }]
                })
            });

            const data = await response.json();
            if (data.candidates && data.candidates[0].content.parts[0].text) {
                return data.candidates[0].content.parts[0].text;
            } else {
                return "申し訳ありませんが、現在応答を処理できません。"; // Default error message
            }
        } catch (error) {
            console.error("Error with chatbot API:", error);
            return "エラーが発生しました。"; // Return error message
        }
    };

    const getMessageStyle = (senderType: string) => {
        return senderType === "User"
            ? "bg-blue-500 text-white self-end rounded-l-lg" // User message on the right side
            : "bg-gray-400 text-white self-start rounded-r-lg"; // Bot message on the left side
    };

    return (
        <div className="flex flex-col w-full max-w-3xl mx-auto bg-white shadow-xl rounded-xl border border-gray-300">
            <div className="bg-gradient-to-r from-blue-600 to-blue-500 text-white p-4 rounded-t-xl">
                <h2 className="text-2xl font-semibold">Chat with Support</h2>
            </div>

            <div className="flex-1 overflow-y-auto p-4 space-y-4 bg-gray-50 rounded-b-xl">
                {/* Render messages */}
                {messages.map((message) => (
                    <div
                        key={message.id}
                        className={`flex p-3 max-w-[80%] ${getMessageStyle(message.senderType)} mb-2`}
                    >
                        <p className="text-sm leading-relaxed">{message.messageContent}</p>
                    </div>
                ))}
            </div>

            <div className="bg-gray-100 p-4 rounded-b-xl flex items-center space-x-3">
                <input
                    type="text"
                    className="flex-1 p-3 rounded-lg border border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-400"
                    value={newMessage}
                    onChange={(e) => setNewMessage(e.target.value)}
                    disabled={loading}
                    placeholder="Type your message here..."
                />
                <button
                    onClick={handleSendMessage}
                    className={`bg-blue-500 text-white p-3 rounded-lg disabled:opacity-50`}
                    disabled={loading}
                >
                    Send
                </button>
            </div>
        </div>
    );
};

export default ChatWindow;

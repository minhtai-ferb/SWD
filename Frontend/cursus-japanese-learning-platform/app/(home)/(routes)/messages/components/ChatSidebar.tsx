import React, { useEffect, useState } from 'react';
import { getChatByUserId } from "@/actions/chat";

type Chat = {
    id: string;
    createdTime: string;
};

type ChatSidebarProps = {
    onSelectChat: (chatId: string) => void;
    userId: string;
};

const ChatSidebar: React.FC<ChatSidebarProps> = ({ onSelectChat, userId }) => {
    const [chats, setChats] = useState<Chat[]>([]);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        const fetchChats = async () => {
            setLoading(true);
            try {
                const response = await getChatByUserId(userId);
                if (Array.isArray(response.data)) {
                    const sortedChats = response.data.sort((a: Chat, b: Chat) => {
                        const timeA = new Date(a.createdTime).getTime();
                        const timeB = new Date(b.createdTime).getTime();
                        return timeA - timeB; 
                    });
                    setChats(sortedChats);  
                } else {
                    console.error("Expected an array of chats, but received:", response);
                }
            } catch (error) {
                console.error("Error fetching chats:", error);
            } finally {
                setLoading(false);
            }
        };

        fetchChats();
    }, [userId]);

    return (
        <div className="w-64 bg-gray-800 text-white h-full p-4 space-y-4 overflow-y-auto">
            <h2 className="text-lg font-semibold">Chats</h2>
            {loading ? (
                <p>Loading...</p>
            ) : (
                chats.length > 0 ? (
                    chats.map((chat) => (
                        <button
                            key={chat.id}
                            onClick={() => onSelectChat(chat.id)}
                            className="flex flex-col items-start bg-gray-700 p-3 rounded-lg hover:bg-gray-600 w-full text-left"
                        >
                            <span className="font-bold text-sm">{chat.id}</span>
                        </button>
                    ))
                ) : (
                    <p>No chats available</p>
                )
            )}
        </div>
    );
};

export default ChatSidebar;

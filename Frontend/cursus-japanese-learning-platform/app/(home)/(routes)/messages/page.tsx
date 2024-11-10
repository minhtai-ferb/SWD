"use client";
import React, { useState, useEffect } from "react";
import ChatWindow from "./components/ChatWindow";
import { createChatForUser, getChatByUserId } from "@/actions/chat";

const Page = () => {
  const [chatId, setChatId] = useState<string | null>(null);
  const userId = "083a56ef-3d5a-4cdc-d90f-08dd00d9ac1e"; // Lấy từ context hoặc từ auth.

  useEffect(() => {
    const fetchChat = async () => {
      try {
        const response = await getChatByUserId(userId);
        const chat = response.data[response.data.length - 1];
        if (chat && chat.id) {
          setChatId(chat.id);
        } else {
          const newChat = await createChatForUser(userId);
          setChatId(newChat.id);
        }
      } catch (error) {
        console.error("[Page] Error fetching or creating chat", error);
      }
    };

    fetchChat();
  }, [userId]);

  return (
    <div className="flex h-screen">
      <div className="flex-1 p-6">
        {chatId ? (
          <ChatWindow chatId={chatId} />
        ) : (
          <div>Loading chat...</div>
        )}
      </div>
    </div>
  );
};

export default Page;

"use client";
import React, { useState, useEffect } from "react";
import ChatWindow from "./components/ChatWindow";
import { createChatForUser, getChatByUserId } from "@/actions/chat";
import ChatSidebar from "./components/ChatSidebar";

const Page = () => {
  const [chatId, setChatId] = useState<string | null>(null);
  const [userId, setUserId] = useState<string | null>(null);

  useEffect(() => {
    if (typeof window !== "undefined") {
      const userData = localStorage.getItem("userData");
      const parsedUserId = userData ? JSON.parse(userData).id : null;
      setUserId(parsedUserId);
    }
  }, []);

  useEffect(() => {
    if (!userId) {
      console.error("[Page] No userId found in localStorage");
      return;
    }

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
      <ChatSidebar onSelectChat={setChatId} userId={userId || ""} />
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

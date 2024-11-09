"use client";

import { Suspense} from "react";
import Cookies from "js-cookie";
import { isLoggedIn } from "@/lib/login-check";
import SearchInput from "@/components/search-input";
import Loader from "@/app/components/Loader";

const Home = () => {

  const token = Cookies.get("jwtToken");

  if (token) {
    isLoggedIn();
  }

  return (
    <>
      <div className="px-6 pt-6 md:hidden md:mb-0 block">
        <SearchInput />
      </div>
      <div className="p-6 space-y-4">
        
      </div>
    </>
  );
};

const HomePage = () => (
  <Suspense
    fallback={
      <div className="flex flex-col items-center justify-center h-screen">
        <Loader />
      </div>
    }
  >
    <Home />
  </Suspense>
);

export default HomePage;

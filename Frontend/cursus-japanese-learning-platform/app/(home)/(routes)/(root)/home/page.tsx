// Home.tsx
"use client";

import { useEffect, useState } from "react";
import Cookies from "js-cookie";
import SearchInput from "@/components/search-input";
import { isLoggedIn } from "@/lib/login-check";


const Home = () => {
  const token = Cookies.get("jwtToken");
  const [searchResults, setSearchResults] = useState<any>(null);

  // Check for login state
  if (token) {
    isLoggedIn();
  }

  useEffect(() => {
    // Custom event listener for search updates
    const handleSearchUpdate = (event: any) => {
      const data = event.detail;
      setSearchResults(data); // Update search results state
    };

    // Listen for the custom searchUpdated event
    window.addEventListener("searchUpdated", handleSearchUpdate);

    // Cleanup the listener on unmount
    return () => {
      window.removeEventListener("searchUpdated", handleSearchUpdate);
    };
  }, []);

  return (
    <>
      <div className="px-6 pt-6 md:hidden md:mb-0 block">
        {/* Pass handleSearchResults to SearchInput to update searchResults */}
        {/* <SearchInput /> */}
      </div>
      <div className="p-6 space-y-4">
        {/* Display the fetched search results */}
        {searchResults ? (
          <div>
            <p className="text-lg font-medium mb-4">
              {/* flag */}
              <p></p>
              Search results <strong>{searchResults.word}</strong>
            </p>
            <div className="flex gap-3">
              <div className="bg-white p-4 rounded-lg shadow-md space-y-2 flex-grow">
                <p className="text-sm text-gray-700">{searchResults.word}</p>
                <p className="text-sm text-gray-700">Meaning: {searchResults.meaning}</p>
              </div>
              <img width={200} src="https://th.bing.com/th/id/R.aff4ceb15704422ff22c17caa5529307?rik=s%2faef%2fupI7VcJQ&pid=ImgRaw&r=0" />
            </div>
          </div>
        ) : (
          <p>No results yet</p>
        )}
      </div>
    </>
  );
};

export default Home;

"use client";

import { Suspense, useState } from "react";
import Cookies from "js-cookie";
import { isLoggedIn } from "@/lib/login-check";
import SearchInput from "@/components/search-input";
import Loader from "@/app/components/Loader";

const Home = () => {
  const [searchResults, setSearchResults] = useState<any>(null);
  const [initState, setInitState] = useState('');
  const token = Cookies.get("jwtToken");

  if (token) {
    isLoggedIn();
  }

  const handleSearchResults = (data: any) => {
    console.log("Fetched data:", data);
    setSearchResults(data);
  };

  return (
    <>
      <div className="px-6 pt-6 md:hidden md:mb-0 block">
        <SearchInput onSearch={handleSearchResults} />
      </div>
      <div className="p-6 space-y-4">
        {/* Display the fetched search results */}
        {searchResults ? (
          searchResults.data ? (
            <div>
              <h3>Translation Result:</h3>
              <p><strong>Word:</strong> {searchResults.word}</p>
              <p><strong>Meaning:</strong> {searchResults.meaning}</p>
            </div>
          ) : (
            <p>No valid translation data found.</p>
          )
        ) : (
          <p>No results found.</p>
        )}
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
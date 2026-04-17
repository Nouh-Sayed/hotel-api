import { useEffect, useMemo, useState } from "react";
import { Link } from "react-router-dom";
import api from "../services/api";

export default function Home() {
  const [hotels, setHotels] = useState([]);
  const [loading, setLoading] = useState(true);

  const [search, setSearch] = useState("");
  const [selectedCity, setSelectedCity] = useState("");
  const [minPrice, setMinPrice] = useState("");
  const [maxPrice, setMaxPrice] = useState("");
  const [rating, setRating] = useState("");

  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);

  const fetchHotels = async () => {
    try {
      setLoading(true);

      const params = new URLSearchParams();
      params.append("page", page);
      params.append("pageSize", 12);

      if (search.trim()) params.append("search", search.trim());
      if (selectedCity) params.append("city", selectedCity);
      if (minPrice) params.append("minPrice", minPrice);
      if (maxPrice) params.append("maxPrice", maxPrice);
      if (rating) params.append("rating", rating);

      const res = await api.get(`/hotels?${params.toString()}`);

      setHotels(res.data.data);
      setTotalPages(res.data.totalPages);
    } catch (err) {
      console.error("Error fetching hotels:", err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchHotels();
  }, [page]);

  const cities = useMemo(() => {
    return [...new Set(hotels.map((hotel) => hotel.city).filter(Boolean))];
  }, [hotels]);

  const handleFilter = () => {
    setPage(1);
    fetchHotels();
  };

  if (loading) {
    return <div className="p-6 text-xl">Loading hotels...</div>;
  }

  return (
    <div className="min-h-screen bg-gray-100 p-6">
      <h1 className="text-3xl font-bold mb-6">Hotels</h1>

      <div className="bg-white p-4 rounded-2xl shadow mb-6 grid grid-cols-1 md:grid-cols-2 lg:grid-cols-5 gap-4">
        <input
          type="text"
          placeholder="Search name or city"
          className="border border-gray-300 rounded-xl px-4 py-2 w-full"
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />

        <select
          className="border border-gray-300 rounded-xl px-4 py-2 w-full"
          value={selectedCity}
          onChange={(e) => setSelectedCity(e.target.value)}
        >
          <option value="">All Cities</option>
          {cities.map((city, index) => (
            <option key={index} value={city}>
              {city}
            </option>
          ))}
        </select>

        <input
          type="number"
          placeholder="Min Price"
          className="border border-gray-300 rounded-xl px-4 py-2 w-full"
          value={minPrice}
          onChange={(e) => setMinPrice(e.target.value)}
        />

        <input
          type="number"
          placeholder="Max Price"
          className="border border-gray-300 rounded-xl px-4 py-2 w-full"
          value={maxPrice}
          onChange={(e) => setMaxPrice(e.target.value)}
        />

        <select
          className="border border-gray-300 rounded-xl px-4 py-2 w-full"
          value={rating}
          onChange={(e) => setRating(e.target.value)}
        >
          <option value="">Any Rating</option>
          <option value="1">1+</option>
          <option value="2">2+</option>
          <option value="3">3+</option>
          <option value="4">4+</option>
          <option value="5">5</option>
        </select>

        <button
          onClick={handleFilter}
          className="lg:col-span-5 bg-blue-600 text-white py-2 rounded-xl hover:bg-blue-700 transition"
        >
          Apply Filters
        </button>
      </div>

      {hotels.length === 0 ? (
        <div className="bg-white rounded-2xl shadow p-6 text-gray-600">
          No hotels found.
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {hotels.map((hotel) => (
            <div
              key={hotel.externalId}
              className="bg-white rounded-2xl shadow-md p-5 hover:shadow-lg transition"
            >
              <h2 className="text-xl font-semibold mb-2">{hotel.name}</h2>
              <p className="text-gray-600 mb-1">{hotel.city}, {hotel.country}</p>
              <p className="text-yellow-600 font-medium mb-1">Rating: {hotel.rating}</p>
              <p className="text-blue-600 font-bold mb-3">
                {hotel.pricePerNightTry} {hotel.currency}
              </p>

              <p className="text-sm text-gray-500 line-clamp-2">{hotel.address}</p>

              <div className="mt-4 flex flex-wrap gap-2">
                {hotel.amenities?.slice(0, 4).map((amenity, index) => (
                  <span
                    key={index}
                    className="bg-gray-200 text-sm px-3 py-1 rounded-full"
                  >
                    {amenity}
                  </span>
                ))}
              </div>

              <Link
                to={`/hotel/${hotel.externalId}`}
                className="inline-block mt-5 bg-blue-600 text-white px-4 py-2 rounded-xl hover:bg-blue-700 transition"
              >
                View Details
              </Link>
            </div>
          ))}
        </div>
      )}

      <div className="flex items-center justify-center gap-4 mt-8">
        <button
          onClick={() => setPage((prev) => Math.max(prev - 1, 1))}
          disabled={page === 1}
          className="bg-gray-300 px-4 py-2 rounded-xl disabled:opacity-50"
        >
          Prev
        </button>

        <span className="font-semibold">
          Page {page} of {totalPages}
        </span>

        <button
          onClick={() => setPage((prev) => Math.min(prev + 1, totalPages))}
          disabled={page === totalPages}
          className="bg-blue-600 text-white px-4 py-2 rounded-xl disabled:opacity-50"
        >
          Next
        </button>
      </div>
    </div>
  );
}
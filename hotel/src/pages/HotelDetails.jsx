import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import api from "../services/api";

export default function HotelDetails() {
  const { id } = useParams();
  const [hotel, setHotel] = useState(null);
  const [loading, setLoading] = useState(true);
  const [mainImage, setMainImage] = useState("");

  useEffect(() => {
    api.get(`/hotels/${id}`)
      .then((res) => {
        setHotel(res.data);
        setMainImage(res.data.images?.[0] || "");
        setLoading(false);
      })
      .catch((err) => {
        console.error(err);
        setLoading(false);
      });
  }, [id]);

  if (loading)
    return <div className="p-10 text-2xl font-semibold">Loading...</div>;

  if (!hotel)
    return <div className="p-10 text-red-600 text-2xl">Hotel not found.</div>;

  return (
    <div className="min-h-screen bg-gray-100 p-6">
      <div className="max-w-7xl mx-auto">

        {/* Main Card */}
        <div className="bg-white rounded-3xl shadow-xl overflow-hidden">

          {/* Hero Image */}
          {mainImage && (
            <img
              src={mainImage}
              alt={hotel.name}
              className="w-full h-[420px] object-cover"
            />
          )}

          <div className="p-8 grid grid-cols-1 lg:grid-cols-3 gap-8">

            {/* Left */}
            <div className="lg:col-span-2">
              <h1 className="text-4xl font-bold mb-4">{hotel.name}</h1>

              <p className="text-lg text-gray-600 mb-2">
                {hotel.city}, {hotel.country}
              </p>

              <p className="text-yellow-500 text-xl font-semibold mb-3">
                ⭐ Rating: {hotel.rating}
              </p>

              <p className="text-3xl text-blue-700 font-bold mb-6">
                {hotel.pricePerNightTry} {hotel.currency}
              </p>

              <p className="text-gray-700 text-lg leading-8 mb-8">
                {hotel.address}
              </p>

              {/* Amenities */}
              <h2 className="text-2xl font-bold mb-4">Amenities</h2>

              <div className="flex flex-wrap gap-3">
                {hotel.amenities?.length > 0 ? (
                  hotel.amenities.map((item, index) => (
                    <span
                      key={index}
                      className="bg-gray-200 px-4 py-2 rounded-full text-sm font-medium"
                    >
                      {item}
                    </span>
                  ))
                ) : (
                  <p className="text-gray-500">No amenities available.</p>
                )}
              </div>
            </div>

            {/* Right Booking Card */}
            <div className="bg-gray-50 rounded-3xl p-6 shadow-md h-fit">
              <h3 className="text-2xl font-bold mb-4">Reserve Now</h3>

              <p className="text-gray-600 mb-4">
                Book this hotel instantly and secure your stay.
              </p>

              <p className="text-3xl text-blue-700 font-bold mb-6">
                {hotel.pricePerNightTry} {hotel.currency}
              </p>

              <Link
                to="/booking"
                className="block w-full text-center bg-green-600 text-white py-3 rounded-2xl text-lg hover:bg-green-700 transition mb-4"
              >
                Book Now
              </Link>

              {hotel.googleMapsSearch && (
                <a
                  href={hotel.googleMapsSearch}
                  target="_blank"
                  rel="noreferrer"
                  className="block w-full text-center bg-blue-600 text-white py-3 rounded-2xl text-lg hover:bg-blue-700 transition"
                >
                  Open Map
                </a>
              )}
            </div>
          </div>
        </div>

        {/* Gallery */}
        {hotel.images?.length > 1 && (
          <div className="mt-8">
            <h2 className="text-2xl font-bold mb-4">Gallery</h2>

            <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
              {hotel.images.map((img, index) => (
                <img
                  key={index}
                  src={img}
                  alt=""
                  onClick={() => setMainImage(img)}
                  className="h-40 w-full object-cover rounded-2xl cursor-pointer hover:scale-105 transition"
                />
              ))}
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
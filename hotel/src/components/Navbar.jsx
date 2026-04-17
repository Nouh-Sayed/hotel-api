import { Link } from "react-router-dom";

export default function Navbar() {
  return (
    <nav className="bg-blue-700 text-white shadow-md">
      <div className="max-w-7xl mx-auto px-6 py-4 flex items-center justify-between">
        <Link to="/" className="text-2xl font-bold">
          Hotel Booking
        </Link>

        <div className="flex gap-6 text-lg">
          <Link to="/" className="hover:text-gray-200 transition">
            Home
          </Link>
          <Link to="/booking" className="hover:text-gray-200 transition">
            Booking
          </Link>
        </div>
      </div>
    </nav>
  );
}
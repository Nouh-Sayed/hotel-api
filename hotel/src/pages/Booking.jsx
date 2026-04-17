import { useState } from "react";
import api from "../services/api";

export default function Booking() {
  const [formData, setFormData] = useState({
    fullName: "",
    phone: "",
    email: "",
    nationality: "",
    identityNumber: "",
    hotelId: "",
    roomId: "",
    checkInDate: "",
    checkOutDate: "",
    adults: 1,
    children: 0,
  });

  const [message, setMessage] = useState("");

  const handleChange = (e) => {
    setFormData((prev) => ({
      ...prev,
      [e.target.name]: e.target.value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setMessage("");

    try {
      const res = await api.post("/bookings/reservation", {
        ...formData,
        hotelId: Number(formData.hotelId),
        roomId: Number(formData.roomId),
        adults: Number(formData.adults),
        children: Number(formData.children),
      });

      setMessage("Booking created successfully!");
      console.log(res.data);
    } catch (error) {
      console.error(error);
      setMessage("Booking failed. Check your data.");
    }
  };

  return (
    <div className="min-h-screen bg-gray-100 p-6">
      <div className="max-w-3xl mx-auto bg-white rounded-2xl shadow-lg p-6">
        <h1 className="text-3xl font-bold mb-6">Book a Room</h1>

        <form onSubmit={handleSubmit} className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <input name="fullName" placeholder="Full Name" className="border p-3 rounded-xl" onChange={handleChange} required />
          <input name="phone" placeholder="Phone" className="border p-3 rounded-xl" onChange={handleChange} required />
          <input name="email" type="email" placeholder="Email" className="border p-3 rounded-xl" onChange={handleChange} required />
          <input name="nationality" placeholder="Nationality" className="border p-3 rounded-xl" onChange={handleChange} />
          <input name="identityNumber" placeholder="Identity Number" className="border p-3 rounded-xl" onChange={handleChange} />
          <input name="hotelId" placeholder="Hotel ID" className="border p-3 rounded-xl" onChange={handleChange} required />
          <input name="roomId" placeholder="Room ID" className="border p-3 rounded-xl" onChange={handleChange} required />
          <input name="checkInDate" type="datetime-local" className="border p-3 rounded-xl" onChange={handleChange} required />
          <input name="checkOutDate" type="datetime-local" className="border p-3 rounded-xl" onChange={handleChange} required />
          <input name="adults" type="number" placeholder="Adults" className="border p-3 rounded-xl" onChange={handleChange} required />
          <input name="children" type="number" placeholder="Children" className="border p-3 rounded-xl" onChange={handleChange} />
          
          <button
            type="submit"
            className="md:col-span-2 bg-blue-600 text-white py-3 rounded-xl hover:bg-blue-700 transition"
          >
            Submit Booking
          </button>
        </form>

        {message && <p className="mt-4 text-lg font-medium">{message}</p>}
      </div>
    </div>
  );
}
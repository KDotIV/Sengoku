import React, { useState } from 'react';
import './App.css';
import { useSelector, useDispatch } from "react-redux";
import {RootStore} from './stores/store';
import {GetEvents} from "./actions/EventsActions";

function App() {
  const dispatch = useDispatch();
  const [eventId, setEventId] = useState("");
  const eventsState = useSelector((state: RootStore) => state.events);
  const handleSubmit = () => dispatch(GetEvents(eventId));
  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => setEventId(event.target.value);

  console.log("Event State:", eventsState);
  return (
    <div className="App">
      <input type="text" onChange={handleChange}/>
      <button onClick={handleSubmit}>Search</button>
    </div>
  );
}

export default App;

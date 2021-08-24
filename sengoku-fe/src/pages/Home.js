import React, { useEffect } from 'react';
import { useDispatch } from "react-redux";
import { loadEvents } from "../actions/eventsAction";

const Home = () =>{
    const dispatch = useDispatch();
    useEffect(() => {
      dispatch(loadEvents());
    });
    return (
        <div>
            <h1>Landed</h1>
            <h2>Welcome To Sengoku</h2>
        </div>
    );
}

export default Home;
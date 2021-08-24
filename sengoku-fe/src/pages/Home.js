import React, { useEffect } from 'react';
import { useDispatch, useSelector } from "react-redux";
import { loadEvents } from "../actions/eventsAction";
//Components
import Events  from "../components/eventComponent";
//Styling
import styled from 'styled-components';
import { motion } from "framer-motion";

const Home = () => {
    const dispatch = useDispatch();
    useEffect(() => {
      dispatch(loadEvents());
    }, [dispatch]);

    const {allEvents} = useSelector((state) => state.events);
    return (
        <div>
            <h1>Landed</h1>
            <h2>Welcome To Sengoku</h2>
            <div>
            <EventList>
                <h2>Current Events</h2>
                <EventsStyle>
                    {allEvents.map((result) => (
                        <Events
                        id={result.event_Id}
                        eventName={result.name}
                        eventGame={result.game}
                        eventLocation={result.city}
                        key={result.event_Id}
                        />
                    ))};
                </EventsStyle>
            </EventList>
            </div>
        </div>
    );
}

const EventList = styled(motion.div)``;

const EventsStyle = styled(motion.div)``;

export default Home;
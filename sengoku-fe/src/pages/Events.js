import React, { useEffect } from 'react';
import { useDispatch, useSelector } from "react-redux";
import { loadEvents } from "../actions/eventsAction";
//Components
import EventComponent  from "../components/eventComponent";
//Styling
import styled from 'styled-components';
import { motion } from "framer-motion";
import { useLocation } from 'react-router';

const Events = () => {
    //get current location
    const location = useLocation();
    const pathId = location.pathname.split("/");
    //FETCH EVENTS
    const dispatch = useDispatch();
    useEffect(() => {
      dispatch(loadEvents());
    }, [dispatch]);

    const {allEvents} = useSelector((state) => state.events);
    return (
    <EventList>
        <h2>Current Events</h2>
        <EventsStyle>
            {allEvents.map((result) => (
                <EventComponent
                id={result.event_Id}
                eventName={result.name}
                eventGame={result.game}
                eventLocation={result.city}
                key={result.event_Id}
                />
            ))}
        </EventsStyle>
    </EventList>
    );
}

const EventList = styled(motion.div)`
    padding: 0rem 5rem;
    h2{
        padding: 5rem 0rem;
    }
`;

const EventsStyle = styled(motion.div)`
    min-height: 80vh;
    display: grid;
    grid-template-columns: repeat(auto-fit,minmax(500px, 1fr));
    grid-column-gap: 3rem;
    grid-row-gap: 5rem;
`;

export default Events;
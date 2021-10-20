import React, { useEffect } from 'react';
import { useDispatch, useSelector } from "react-redux";
import { loadEvents } from "../actions/eventsAction";
//Components
import Events  from "../components/eventComponent";
import Players from "../components/playerComponent";
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
                <HeaderStyles>
                    <div className="bar">
                        <Logo>
                            <p>Sengoku</p>
                        </Logo>
                    </div>
                    <div className="sub-bar">
                        <h3>Search</h3>
                    </div>
                </HeaderStyles>
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
                    ))}
                </EventsStyle>
            </EventList>
            </div>
        </div>
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

const Logo = styled.h1`
    height: auto;
    font-size: 2rem;
    margin-left: 1rem;
    position: relative;
    z-index: 2;
    background: LightCoral;
    overflow: hidden;
    p {
        color: white;
        text-align: center;
        text-decoration: none;
        padding: 0.5rem 1rem;
    }
`;

const HeaderStyles = styled.header`
  .bar {
    border-bottom: 10px solid var(--black, black);
    display: grid;
    grid-template-columns: auto 1fr;
    justify-content: space-between;
    align-items: center;
  }

  .sub-bar {
    display: grid;
    grid-template-columns: 1fr auto;
    border-bottom: 1px solid var(--black, black);
    h3 {
        margin-left: 1rem;
    }
  }
`;

export default Home;
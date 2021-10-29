import React, { useEffect } from 'react';
import { useDispatch, useSelector } from "react-redux";
//Components
import LegendComponent  from "../components/legendsComponent";
import LegendDetail from '../components/legendDetailComponent';
//Styling
import styled from 'styled-components';
import { motion } from "framer-motion";
import { useLocation } from 'react-router';
import { loadLegends } from '../actions/legendsAction';

const Legends = () => {
    //get current location
    const location = useLocation();
    const pathId = location.pathname.split("/")[2];
    console.log(pathId);
    //FETCH LEGENDS
    const dispatch = useDispatch();
    useEffect(() => {
      dispatch(loadLegends());
    }, [dispatch]);

    const {allLegends} = useSelector((state) => state.legends);
    return (
    <>
        <LegendList>
        {pathId && <LegendDetail />}
            <h2>New Legends</h2>
            <LegendsStyle>
                {allLegends.map((result) => (
                    <LegendComponent
                    id={result.legendId}
                    subject={result.subject}
                    summary={result.summary}
                    game={result.game}
                    key={result.legendId}
                    />
                ))}
            </LegendsStyle>
        </LegendList>
    </>
    );
}

const LegendList = styled(motion.div)`
    padding: 0rem 5rem;
    h2{
        padding: 5rem 0rem;
    }
`;

const LegendsStyle = styled(motion.div)`
    min-height: 80vh;
    display: grid;
    grid-template-columns: repeat(auto-fit,minmax(500px, 1fr));
    grid-column-gap: 3rem;
    grid-row-gap: 5rem;
`;

export default Legends;
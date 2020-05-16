import React from 'react';
import logo from './logo.png';
import { Typography } from '@material-ui/core';
import Grid from '@material-ui/core/Grid';
import SelectPerson from './person-select'
import ViewSelect from './view-select';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';


const Header = (props) => (
    <AppBar position="sticky" color="inherit">
        <Toolbar>
            <Grid container spacing={5} direction="row" justify="space-between" alignItems="center">
                <Grid item>
                    <Grid container spacing={2} direction="row" justify="space-between" alignItems="center">
                        <Grid item>
                            <img src={logo} height="40" className="logo" alt="logo"/>
                        </Grid>
                        <Grid item>
                            <Typography variant="h4">
                                {props.subtitle}
                            </Typography>
                        </Grid>
                    </Grid>
                </Grid>
                <Grid item>
                    <SelectPerson id={props.activePerson} selectedPerson={props.selectedPerson}/>
                </Grid>
                <Grid item>
                    <ViewSelect />
                </Grid>
            </Grid>
        </Toolbar>
    </AppBar>
);

export default Header;
import React from 'react';
import FamilyTree from '../FamilyTree'
import PersonDetails from '../PersonDetails'
import EditDetails from '../EditDetails'
import { makeStyles } from '@material-ui/core/styles';
import Header from '../Header';
import Upload from '../UploadImage';
import { useSelector } from 'react-redux'
import { ViewMode } from '../actions';

const useStyles = makeStyles((theme) => ({
  root: {
    flexGrow: 1,
  },
  paper: {
    padding: theme.spacing(2),
    textAlign: 'center',
    color: theme.palette.text.secondary,
  },
}));

const ViewSwitch = (props) => {
  const view = useSelector(state => state.viewMode )
  switch(view) {
    case ViewMode.SHOW_DETAIL:
      return <PersonDetails id={props.activePerson} selectedPerson={props.selectedPerson}/>
    case ViewMode.SHOW_EDIT:
      return <EditDetails id={props.activePerson}/>
    case ViewMode.SHOW_UPLOAD:
      return <Upload/>
    default:
      return <FamilyTree id={props.activePerson} selectedPerson={props.selectedPerson}/> 
  }
}

const AppPresentation = (props) => {
  const classes = useStyles();

  return (
  <div className={classes.root}>
      <Header subtitle="Family Archive" activePerson={props.activePerson} selectedPerson={props.selectedPerson}/>
      {ViewSwitch(props)}
  </div>
  );
}

export default AppPresentation; 
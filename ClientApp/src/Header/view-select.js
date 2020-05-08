import React from 'react'
import ViewLink from '../containers/ViewLink';
import { ViewMode } from '../actions';
import { useSelector } from 'react-redux'
import ToggleButtonGroup from '@material-ui/lab/ToggleButtonGroup';

const ViewSelect = () => {
  return (
  <div>
    <ToggleButtonGroup exclusive>
      <ViewLink filter={ViewMode.SHOW_FAMILY_TREE}>Family Tree</ViewLink>
      <ViewLink filter={ViewMode.SHOW_DETAIL}>Detail</ViewLink>
      <ViewLink filter={ViewMode.SHOW_EDIT}>Edit</ViewLink>
      <ViewLink filter={ViewMode.SHOW_UPLOAD}>UPLOAD</ViewLink>
    </ToggleButtonGroup>
  </div>
  );
}

export default ViewSelect
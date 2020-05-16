/*
 * action types
 */

export const SET_VIEW_MODE_FILTER = 'SET_VIEW_MODE_FILTER'

/*
 * other constants
 */

export const ViewMode = {
  SHOW_FAMILY_TREE: 'SHOW_FAMILY_TREE',
  SHOW_DETAIL: 'SHOW_DETAIL',
  SHOW_EDIT: 'SHOW_EDIT',
  SHOW_UPLOAD: 'SHOW_UPLOAD'
}

/*
 * action creators
 */

export function setViewMode(filter) {
  return { type: SET_VIEW_MODE_FILTER, filter }
}
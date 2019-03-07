grammar FUSQL;
/*
 * Parser Rules
 */
 query			: command NEWLINE ;
 command		: FIND group ;
 group			: GROUP name USING (attribute)+ ;
 attribute		: (name EQUAL value) | (name NOT_EQUAL value);
 name			: WORD ;
 value			: WORD ;

/*
 * Lexer Rules
 */
fragment G		: ('G'|'g') ;
fragment R		: ('R'|'r') ;
fragment O		: ('O'|'o') ;
fragment U		: ('U'|'u') ;
fragment P		: ('P'|'p') ;
fragment S		: ('S'|'s') ;
fragment I		: ('I'|'i') ;
fragment N		: ('N'|'n') ;
fragment F		: ('F'|'f') ;
fragment D		: ('D'|'d') ;
UPPERCASE		: [A-Z] ;
LOWERCASE		: [a-z] ;
EQUAL			: '=' ;
NOT_EQAUL		: '!=' ;
GROUP			: G R O U P ;
USING			: U S I N G ;
FIND			: F I N D ;
WORD			: (UPPERCASE | LOWERCASE)+ ;
NEWLINE			: '\r' '\n' | '\n' | '\r';
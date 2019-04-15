grammar FUSQL;
/*
 * Parser Rules
 */
 query					: command NEWLINE ;
 command				: (create) | (delete) | (classify) | (find) | (check) | (identify) ;
 //check					: CHECK for using with from ;
 //for					: FOR name ;
 using					: USING string ;

 with					: WITH name ;
 create					: CREATE ((classification) | (checker)) (where)? ;
 classification			: CLASSIFICATION name USING (column)+ TO goal from;
 checker				: CHECKER name USING (column)+ TO goal from;
 check					: CHECK (((term)+ USING name) | (DATA USING name from (where)?));
 delete					: DELETE ((deleteclassification) | (deletechecker));
 deleteclassification	: CLASSIFICATION name ;
 deletechecker			: CHECKER name ;
 classify				: CLASSIFY (((term)+ USING name) | (DATA USING name from (where)?  )) ;
 term					: string IN column ;
 find					: FIND groups from (where)?;
 groups					: number CLUSTERS USING (column)+ ; // Groups in this case is analogus to clusters
 from					: FROM name ;
 where					: WHERE (conditions)+ ;
 dataset_info			: INFO ;
 conditions				: ((name EQUAL value) | (name NOT_EQUAL value) | 
							(name GREATER_THAN value) | (name LESS_THAN value)) (COMMA)?;
 
 column			: WORD ;
 goal			: WORD ;
 name			: WORD ;
 number			: NUMBER ;
 value			: TEXT | NUMBER | STRING ;
 string			: STRING;
 description	: WORD ;
 and			: AND name ;
 identify		: IDENTIFY description using with from ;

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
fragment M		: ('M'|'m') ;
fragment A		: ('A'|'a') ;
fragment C		: ('C'|'c') ;
fragment E		: ('E'|'e') ;
fragment T		: ('T'|'t') ;
fragment L		: ('L'|'l') ;
fragment H		: ('H'|'h') ;
fragment K		: ('K'|'k') ;
fragment W		: ('W'|'w') ;
fragment Y		: ('Y'|'y') ;
UPPERCASE		: [A-Z] ;
LOWERCASE		: [a-z] ;
EQUAL			: '=' | ('equal') ;
NOT_EQUAL		: '!='| ('not equal')  ;
GREATER_THAN	: '>' | ('greater than') ;
LESS_THAN		: '<' | ('less than');
IDENTIFY		: I D E N T I F Y ;
CLASSIFY		: C L A S S I F Y ;
ENTRIES			: E N T R I E S ;
DATA			: D A T A;
FOR				: F O R ;
WITH			: W I T H ;
IN				: I N ;
CLUSTERS		: C L U S T E R S ;
CREATE			: C R E A T E ;
DELETE			: D E L E T E ;
ALL				: A L L ;
CHECKER			: C H E C K E R ;
CHECK			: C H E C K ;
CLASSIFICATION	: C L A S S I F I C A T I O N ;
TO				: T O ;
CLASS			: C L A S S ;
FROM			: F R O M ;
WHERE			: W H E R E ;
GET				: G E T ;
INFO			: I N F O ;
USING			: U S I N G ;
FIND			: F I N D ;
AND				: A N D ;
OR				: O R ;
COMMA			: ',' ;
STRING			: '\'' (~['"])* '\'';
WORD			: (UPPERCASE | LOWERCASE)+ ;
TEXT			: ('\'')(WORD)('\'') ;
NUMBER			: [0-9]+ ;
NEWLINE			: '\r' '\n' | '\n' | '\r';
WHITESPACE		: (' ')+ -> skip ;

